let Catalog;
document.addEventListener("DOMContentLoaded", function () {
  Catalog = new ProductsCatalog();
});

function UpdatePages(page) {
  Catalog.UpdatePages(page);
}
