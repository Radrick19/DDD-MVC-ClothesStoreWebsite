class Menu {
  MainMenu;
  IsHovered;
  CollectionLinksHandler;
  SubcategoriesLinksHandler;
  SelectedCategoryId;
  constructor() {
    let self = this;
    this.MainMenu = document.querySelector(".drop-main-menu");
    this.CollectionLinksHandler = this.MainMenu.querySelector(
      ".collections-handler"
    );
    this.SubcategoriesLinksHandler = this.MainMenu.querySelector(
      ".subcategories-handler"
    );
    for (const link of document.querySelectorAll(".main-menu-category-link")) {
      link.addEventListener("mouseover", function () {
        self.IsHovered = true;
        if (self.SelectedCategoryId != null) {
          let prevSelectedCategoryId = self.SelectedCategoryId;
          self.SelectedCategoryId = CategoryType[link.id];
          if (
            prevSelectedCategoryId != null &&
            prevSelectedCategoryId != self.SelectedCategoryId
          ) {
            self.UpdateLinks();
          }
        } else {
          self.SelectedCategoryId = CategoryType[link.id];
          self.UpdateLinks();
        }
        self.ShowMenu();
      });
      link.addEventListener("mouseout", async function () {
        self.IsHovered = false;
        await self.HideMenu();
      });
    }
    for (const link of document.querySelectorAll(".category-name")) {
      link.addEventListener("mouseover", function () {
        for (const link of document.querySelectorAll(".hovered")) {
          link.firstChild.style.borderStyle = "none";
          link.classList.remove("hovered");
        }
        link.firstChild.style.borderStyle = "solid";
        link.classList.add("hovered");
      });
    }
    this.MainMenu.addEventListener("mouseover", function () {
      self.IsHovered = true;
    });
    this.MainMenu.addEventListener("mouseout", function () {
      self.IsHovered = false;
      self.HideMenu();
    });
  }

  ShowMenu() {
    this.MainMenu.style.display = "flex";
  }

  UpdateLinks() {
    let self = this;
    let subcategoriesUrl =
      "/getsubcategories/" + this.SelectedCategoryId + "/" + false;
    let collectionsUrl =
      "/getcollections/" + this.SelectedCategoryId + "/" + false;

    $.ajax({
      type: "GET",
      dataType: "HTML",
      url: subcategoriesUrl,
      success: function (data) {
        self.SubcategoriesLinksHandler.innerHTML = data;
      },
    });

    $.ajax({
      type: "GET",
      dataType: "HTML",
      url: collectionsUrl,
      success: function (data) {
        self.CollectionLinksHandler.innerHTML = data;
      },
    });
  }

  async HideMenu() {
    if (!this.IsHovered) {
      await new Promise((resolve) => setTimeout(resolve, 250));
    }
    if (!this.IsHovered) {
      this.MainMenu.style.display = "none";
      for (const link of document.querySelectorAll(".hovered")) {
        link.firstChild.style.borderStyle = "none";
        link.classList.remove("hovered");
      }
    }
  }
}
