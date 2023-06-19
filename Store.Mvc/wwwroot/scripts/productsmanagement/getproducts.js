let categoryId = 0;
let totalPage = 1;
let totalClickedElemenet;

document.addEventListener("DOMContentLoaded", function () {
  UpdateProducts(totalPage);
});

function ChangePage(page) {
  totalPage = page;
  UpdateProducts(totalPage, categoryId);
}

function ChangeCategory(clickedElement, id) {
  if (totalClickedElemenet != null) {
    totalClickedElemenet.style.textDecoration = "none";
  }
  totalClickedElemenet = clickedElement;
  totalClickedElemenet.style.textDecoration = "underline";
  categoryId = id;
  totalPage = 1;
  UpdateProducts(totalPage, categoryId);
}

function UpdateProducts(totalPage, categoryId) {
  $.ajax({
    type: "GET",
    dataType: "html",
    url:
      "/Products/ProductsTable?totalPage=" +
      totalPage +
      "&categoryId=" +
      categoryId,
    success: function (data) {
      if (data != null) {
        document.querySelector(".manage-wrapper").innerHTML = data;
      }
    },
  });
  $.ajax({
    type: "GET",
    dataType: "html",
    url:
      "/Products/PaginationLine?totalPage=" +
      totalPage +
      "&categoryId=" +
      categoryId,
    success: function (data) {
      document.querySelector(".pagination-line").innerHTML = data;
    },
  });
}
