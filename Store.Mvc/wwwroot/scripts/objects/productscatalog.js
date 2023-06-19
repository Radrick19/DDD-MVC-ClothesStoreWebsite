class ProductsCatalog {
  SortingType = 1;
  CategoryId;
  SubcategoryId;
  CollectionId;
  SearchText;
  SearchInput;
  TotalPage = 1;
  ProductsHandler;
  PagintationLineHandler;
  ItemsCountHandler;

  constructor() {
    let self = this;
    this.CategoryId = $("#CategoryId").val();
    this.SubcategoryId = $("#SubcategoryId").val();
    this.CollectionId = $("#CollectionId").val();
    this.SearchText = $("#SearchText").val();
    this.SearchInput = $(".search-input");
    this.ProductsHandler = document.querySelector(".grid-content");
    this.PagintationLineHandler = document.querySelector(".pagination-line");
    this.ItemsCountHandler = document.querySelector(".count");
    this.GetProductItems();
    this.GetPagintationLine();
    this.GetProductsCount();
    $("#sorting-selection")
      .select2({
        minimumResultsForSearch: -1,
        width: "200px",
        autocomplete: "off",
      })
      .on("select2:select", function (e) {
        self.SortingType = $("#sorting-selection").val();
        self.TotalPage = 1;
        self.GetProductItems();
        self.GetPagintationLine();
      });
  }

  UpdatePages(page) {
    this.TotalPage = page;
    this.GetProductItems();
    this.GetPagintationLine();
  }

  GetProductItems() {
    let url;
    let self = this;
    if (this.SearchText != "") {
      url =
        "/catalog/productitems/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId +
        "/" +
        this.SearchText +
        "/" +
        this.TotalPage +
        "/" +
        this.SortingType;
    } else {
      url =
        "/catalog/productitems/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId +
        "/" +
        this.TotalPage +
        "/" +
        this.SortingType;
    }
    $.ajax({
      type: "GET",
      dataType: "html",
      url: url,
      success: function (data) {
        if (data != null && data != "") {
          self.ProductsHandler.innerHTML = data;
        }
      },
    });
    this.SearchInput.attr('value', this.SearchText);
  }

  GetProductsCount() {
    let url;
    let self = this;
    if (this.SearchText == null) {
      url =
        "/catalog/itemscount/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId;
    } else {
      url =
        "/catalog/itemscount/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId +
        "/" +
        this.SearchText;
    }
    $.ajax({
      type: "GET",
      dataType: "html",
      url: url,
      success: function (data) {
        self.ItemsCountHandler.innerHTML = data;
      },
    });
  }

  GetPagintationLine() {
    let self = this;
    let url;
    if (this.SearchText != "") {
      url =
        "/catalog/paginationline/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId +
        "/" +
        this.SearchText +
        "/" +
        this.TotalPage;
    } else {
      url =
        "/catalog/paginationline/" +
        this.CategoryId +
        "/" +
        this.SubcategoryId +
        "/" +
        this.CollectionId +
        "/" +
        this.TotalPage;
    }
    $.ajax({
      type: "GET",
      dataType: "html",
      url: url,
      success: function (data) {
        self.PagintationLineHandler.innerHTML = data;
      },
    });
  }
}
