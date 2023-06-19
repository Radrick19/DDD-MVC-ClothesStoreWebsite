class MobileMenu {
  MobileMenuHandler;
  PreviousPageButton;
  TypeOfSearch;
  TypeOfSearchCategory;
  IsOpen;
  CategorySelectionDiv;
  SearchTypeSelectionDiv;
  FilterSelectionDiv;
  BackbuttonAndInfoDiv;
  NavigationCategoryName;
  NavigationSearchTypeName;
  FilterSelectionDiv;

  constructor() {
    let self = this;
    this.MobileMenuHandler = document.querySelector(".button-drop-main-menu");
    this.CategorySelectionDiv = this.MobileMenuHandler.querySelector(
      ".category-selection"
    );
    this.SearchTypeSelectionDiv = this.MobileMenuHandler.querySelector(
      ".search-type-selection"
    );
    this.FilterSelectionDiv =
      this.MobileMenuHandler.querySelector(".filter-selection");
    this.PreviousPageButton =
      this.MobileMenuHandler.querySelector(".backbutton");
    this.BackbuttonAndInfoDiv = this.MobileMenuHandler.querySelector(
      ".backbutton-and-info"
    );
    this.NavigationCategoryName = this.MobileMenuHandler.querySelector(
      ".selected-category-name"
    );
    this.NavigationSearchTypeName = this.MobileMenuHandler.querySelector(
      ".selected-search-type-name"
    );
    this.CategorySelectionDiv.style.display = "flex";
    for (const categorySelect of this.MobileMenuHandler.querySelectorAll(
      ".select-category"
    )) {
      categorySelect.addEventListener("click", function () {
        self.NavigationCategoryName.innerHTML = categorySelect.innerHTML;
        self.SelectCategory(categorySelect.id);
      });
    }
    for (const searchTypeSelect of this.MobileMenuHandler.querySelectorAll(
      ".select-search-type"
    )) {
      searchTypeSelect.addEventListener("click", function () {
        self.NavigationSearchTypeName.innerHTML = searchTypeSelect.innerHTML;
        self.SelectSearchType(searchTypeSelect.id);
      });
    }
  }

  Open() {
    this.MobileMenuHandler.style.display = "flex";
    this.IsOpen = true;
  }

  Close() {
    this.MobileMenuHandler.style.display = "none";
    this.IsOpen = false;
  }

  SelectCategory(categoryName) {
    this.TypeOfSearchCategory = CategoryType[categoryName];
    this.CategorySelectionDiv.style.display = "none";
    this.SearchTypeSelectionDiv.style.display = "flex";
    this.BackbuttonAndInfoDiv.style.display = "flex";
    this.UpdatePreviousButtonValues(WindowType.OnSearchType);
  }

  SelectSearchType(searchType) {
    let self = this;
    this.TypeOfSearch = SearchType[searchType];
    this.SearchTypeSelectionDiv.style.display = "none";
    this.FilterSelectionDiv.style.display = "flex";
    this.UpdatePreviousButtonValues(WindowType.OnFilter);
    let url;
    if (this.TypeOfSearch == 1) {
      url = "/getsubcategories/" + self.TypeOfSearchCategory + "/" + true;
    } else {
      url = "/getcollections/" + self.TypeOfSearchCategory + "/" + true;
    }

    $.ajax({
      type: "GET",
      dataType: "HTML",
      url: url,
      success: function (data) {
        self.FilterSelectionDiv.innerHTML = data;
      },
    });
  }

  UpdatePreviousButtonValues(windowType) {
    let self = this;
    this.PreviousPageButton.replaceWith(
      this.PreviousPageButton.cloneNode(true)
    );
    this.PreviousPageButton =
      this.MobileMenuHandler.querySelector(".backbutton");
    if (windowType == WindowType.OnSearchType) {
      this.PreviousPageButton.addEventListener("click", function () {
        self.CategorySelectionDiv.style.display = "flex";
        self.SearchTypeSelectionDiv.style.display = "none";
        self.BackbuttonAndInfoDiv.style.display = "none";
      });
    } else if (windowType == WindowType.OnFilter) {
      this.PreviousPageButton.addEventListener("click", function () {
        self.CategorySelectionDiv.style.display = "none";
        self.SearchTypeSelectionDiv.style.display = "flex";
        self.NavigationSearchTypeName.innerHTML = "";
        self.FilterSelectionDiv.innerHTML = "";
        self.UpdatePreviousButtonValues(WindowType.OnSearchType);
      });
    }
  }
}

const WindowType = {
  OnSearchType: 0,
  OnFilter: 1,
};

const SearchType = {
  ByCollections: 0,
  BySubcategories: 1,
};
