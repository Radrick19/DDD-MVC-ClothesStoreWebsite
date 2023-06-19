let categoryChanged;
let subcategoryChanged;
document.addEventListener("DOMContentLoaded", function () {
  $("#collection-form-control").select2();
  $(".multi-select").select2({
    width: "100%",
    closeOnSelect: false,
  });
  let subcategoriesSelect = $("#subcategory-form-control").select2({
    width: "100%",
    ajax: {
      transport: (data, success, failure) => {
        $.ajax({
          type: "GET",
          url:
            "/Category/GetAllSubcategories?categoryId=" +
            $("#categry-form-control").val(),
          success: function (data) {
            success({
              results: data,
            });
          },
        }).then(function (data) {
          subcategoriesSelect.empty().trigger("change");
        });
      },
      cache: true,
    },
    templateResult: (data) => data.displayName,
    templateSelection: (data) => formatSubcategoryState(data),
  });

  let categorySelect = $("#categry-form-control")
    .select2({
      width: "100%",
      ajax: {
        transport: (data, success, failure) => {
          $.ajax({
            type: "GET",
            url: "/Category/GetAllCategories",
            success: function (data) {
              success({
                results: data,
              });
            },
          }).then(function (data) {
            categorySelect.empty().trigger("change");
          });
        },
      },
      templateResult: (data) => data.displayName,
      templateSelection: (data) => formatCategoryState(data),
    })
    .on("select2:select", (e) => {
      subcategoriesSelect.val(null).trigger("change");
      subcategoriesSelect.removeAttr("disabled");
    })
    .on("select2:open", (e) => {
      subcategoriesSelect.empty().trigger("change");
      categorySelect.empty().trigger("change");
    });

  if (
    typeof selectedCategoryId != "undefined" &&
    typeof selectedSubcategoryId != "undefined"
  ) {
    var categoryOpt = new Option(
      selectedCategoryName,
      selectedCategoryId,
      true,
      true
    );
    categorySelect.append(categoryOpt);

    var subcategoryOpt = new Option(
      selectedSubcategoryName,
      selectedSubcategoryId,
      true,
      true
    );
    subcategoriesSelect.append(subcategoryOpt);
  }
});

function formatCategoryState(state) {
  if (typeof selectedCategoryId == "undefined" || categoryChanged == true) {
    return state.displayName;
  } else {
    categoryChanged = true;
    return selectedCategoryName;
  }
}

function formatSubcategoryState(state) {
  if (
    typeof selectedSubcategoryId == "undefined" ||
    subcategoryChanged == true
  ) {
    return state.displayName;
  } else {
    subcategoryChanged = true;
    return selectedSubcategoryName;
  }
}
