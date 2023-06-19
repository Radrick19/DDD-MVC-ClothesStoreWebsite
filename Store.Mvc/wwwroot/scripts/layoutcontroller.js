document.addEventListener("DOMContentLoaded", function () {
  let mobileSearchButton;
  let mobileSearchCloseButton;
  let mobileSearchHandler;
  let mobileMenuButton;
  let mobileMenuHandler;

  menuHandler = new Menu();

  mobileMenuButton = document.querySelector(".menu-button");
  mobileMenuHandler = new MobileMenu();

  mobileMenuButton.addEventListener("click", function () {
    if (mobileMenuHandler.IsOpen) {
      mobileMenuHandler.Close();
    } else {
      mobileMenuHandler.Open();
    }
  });

  mobileSearchHandler = document.querySelector(".mobile-search");
  mobileSearchButton = document.querySelector(".mobile-search-button");
  mobileSearchCloseButton = mobileSearchHandler.querySelector(
    ".close-mobile-search-button"
  );

  mobileSearchButton.addEventListener("click", function () {
    mobileSearchHandler.style.display = "flex";
  });

  mobileSearchCloseButton.addEventListener("click", function () {
    mobileSearchHandler.style.display = "none";
  });
});

const CategoryType = {
  Women: 1,
  Men: 2,
  Kids: 3,
  Baby: 4,
};
