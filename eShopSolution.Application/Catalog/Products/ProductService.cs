using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShopSolution.Utilities.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Application.Common;
using eShopSolution.ViewModels.Catalog.ProductImages;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly eShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(eShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //1. Query
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Catergories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //2. Filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            //VD muon lay trang 2 pageIndex = 2 thì Skip (2-1)*10 = 10 records và Take 10 record tiếp theo
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)  //ToListAsync thi phai sua lai await
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };

            return pageResult;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. query join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic //thực hiện Products leftjoin ProductInCategories
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Catergories on pic.CategoryId equals c.Id into picc // thực hiện Catergories leftjoin ProductInCategories
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => request.CategoryId == x.pic.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();
            //VD muon lay trang 2 pageIndex = 2 thì Skip (2-1)*10 = 10 records và Take 10 record tiếp theo
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)  //ToListAsync thi phai sua lai await
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };

            return pageResult;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                SeoAlias = request.SeoAlias,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation ()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };
            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId); //Lay ra danh sach images cua Product đó
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath); // Xóa hết từng image
            }

            _context.Products.Remove(product); //khi Remove 1 product thì EF remove trước đó các đối tượng con của nó(VD: <List>ProductTranslations), rồi remove product
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null || productTranslations == null) throw new eShopException($"Cannot find a product: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;
            //productTranslations.LanguageId = request.LanguageId; //up date LanguageId là vô nghĩa

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync(); //Tra ve so ban ghi da duoc affect
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            //1. Query
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        where p.Id == productId && pt.LanguageId == languageId
                        select new { p, pt };
            var categories = await (from c in _context.Catergories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();
            //2. Map vao obj ProductViewModel
            var productVM = await query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                Categories = categories
            }).FirstOrDefaultAsync();

            return productVM;
        }

        /***
         * Upload file từ client vào folder trên server
         *
         */

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException($"Cannot find an image: {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException($"Cannot find an image: {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
                productImage.Caption = request.Caption;
                productImage.SortOrder = request.SortOrder;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException($"Cannot find an image: {imageId}");
            var viewModel = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                FileSize = productImage.FileSize,
                Id = productImage.Id,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder
            };
            return viewModel;
        }

        public async Task<ApiResult<bool>> CategoryAssisgn(int productId, CategoryAssignRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException($"Cannot find a product: {productId}");

            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories.FirstOrDefaultAsync(x => x.ProductId == productId && x.CategoryId == int.Parse(category.Id));

                if (productInCategory != null && category.Selected == false) //Kiem tra nếu product đang được gán category, category ko duoc chon
                {
                    _context.ProductInCategories.Remove(productInCategory); // thì bỏ gán
                }
                else if (productInCategory == null && category.Selected == true) //Kiem tra nếu product đang chưa được gán category,
                {
                    productInCategory = new ProductInCategory()
                    {
                        ProductId = productId,
                        CategoryId = int.Parse(category.Id)
                    };
                    _context.ProductInCategories.Add(productInCategory);  //thì gán vào
                }
            }

            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(string langguageId, int take)
        {
            //1. query join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic //thực hiện Products leftjoin ProductInCategories
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Catergories on pic.CategoryId equals c.Id into picc // thực hiện Catergories leftjoin ProductInCategories
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == langguageId && (pi == null || pi.IsDefault == true) && p.IsFeatured == true
                        select new { p, pt, pic, pi };
            //2. filter

            //3. Paging
            int totalRow = await query.CountAsync();
            //VD muon lay trang 2 pageIndex = 2 thì Skip (2-1)*10 = 10 records và Take 10 record tiếp theo
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductViewModel>> GetLastestProducts(string langguageId, int take)
        {
            //1. query join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic //thực hiện Products leftjoin ProductInCategories
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Catergories on pic.CategoryId equals c.Id into picc // thực hiện Catergories leftjoin ProductInCategories
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == langguageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pic, pi };
            //2. filter

            //3. Paging
            int totalRow = await query.CountAsync();
            //VD muon lay trang 2 pageIndex = 2 thì Skip (2-1)*10 = 10 records và Take 10 record tiếp theo
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();

            return data;
        }
    }
}