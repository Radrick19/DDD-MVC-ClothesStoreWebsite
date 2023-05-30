let totalSortingType = 1;
document.addEventListener('DOMContentLoaded', function () {
    $('#sorting-selection').select2({
        minimumResultsForSearch: -1,
        width: '200px',
        autocomplete: 'off'
    }).on('select2:select', function (e) {
        totalSortingType = $('#sorting-selection').val();
        UpdateProducts(categoryId, subcategoryId, collectionId, searchText, totalPage, searchInput, totalSortingType);
        });
    let categoryId = $("#CategoryId").val();
    let subcategoryId = $("#SubcategoryId").val();
    let collectionId = $("#CollectionId").val();
    let searchText = $("#SearchText").val();
    let searchInput = $(".search-input");
    let totalPage = 1;
    let url;
    if (searchText == null) {
        url = '/catalog/itemscount/' + categoryId + '/' + subcategoryId + '/' + collectionId
    }
    else {
        url = '/catalog/itemscount/' + categoryId + '/' + subcategoryId + '/' + collectionId + '/' + searchText
    };
    $.ajax({
        type: 'GET',
        dataType: 'html',
        url: url,
        success: function (data) {
            document.querySelector('.count').innerHTML = data;
        }
    })
    UpdateProducts(categoryId, subcategoryId, collectionId, searchText, totalPage, searchInput, totalSortingType);
})


function ChangePage(page) {
    totalPage = page;
    categoryId = $("#CategoryId").val();
    subcategoryId = $("#SubcategoryId").val();
    collectionId = $("#CollectionId").val();
    searchText = $("#SearchText").val();
    searchInput = $(".search-input");
    UpdateProducts(categoryId, subcategoryId, collectionId, searchText, totalPage, searchInput, totalSortingType);
}

function UpdateProducts(categoryId, subcategoryId, collectionId, searchText, totalPage, searchInput, sortType) {
    let url;
    $('#sorting-selection').val(null);
    if (searchText != "") {
        url = '/catalog/productitems/' + categoryId + '/' + subcategoryId + '/' + collectionId + '/' + searchText + '/' + totalPage + '/' + sortType
    }
    else {
        url = '/catalog/productitems/' + categoryId + '/' + subcategoryId + '/' + collectionId + '/' + totalPage + '/' + sortType
    };
    $.ajax({
        type: 'GET',
        dataType: 'html',
        url: url,
        success: function (data) {
            if (data != null) {
                document.querySelector('.grid-content').innerHTML = data;
            }
        }
    })
    if (searchText != "") {
        url = '/catalog/paginationline/' + categoryId + '/' + subcategoryId + '/' + collectionId + '/' + searchText + '/' + totalPage
    }
    else {
        url = '/catalog/paginationline/' + categoryId + '/' + subcategoryId + '/' + collectionId + '/' + totalPage
    };
    $.ajax({
        type: 'GET',
        dataType: 'html',
        url: url,
        success: function (data) {
            document.querySelector('.pagination-line').innerHTML = data;
        }
    })
    searchInput.attr('value', searchText);
}
