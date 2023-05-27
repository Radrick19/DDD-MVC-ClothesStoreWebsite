const mobileMenu = document.querySelector(".button-drop-main-menu")
const menuButton = document.querySelector(".menu-button")

const selectWomenLink = document.querySelector(".select-women");
const selectMenLink = document.querySelector(".select-men");
const selectKidsLink = document.querySelector(".select-kids");
const selectBabyLink = document.querySelector(".select-baby");

const selectByCollectionLink = document.querySelector(".select-bycollection");
const selectByCategory = document.querySelector(".select-bycategory");

const categorySelection = document.querySelector(".category-selection");
const searchTypeSelection = document.querySelector(".search-type-selection")

const subcategorySelectionWomen = document.querySelector(".subcategory-selection-women")
const subcategorySelectionMen = document.querySelector(".subcategory-selection-men")
const subcategorySelectionKids = document.querySelector(".subcategory-selection-kids")
const subcategorySelectionBaby = document.querySelector(".subcategory-selection-baby")

const collectionSelectionWomen = document.querySelector(".collection-selection-women")
const collectionSelectionMen = document.querySelector(".collection-selection-men")
const collectionSelectionKids = document.querySelector(".collection-selection-kids")
const collectionSelectionBaby = document.querySelector(".collection-selection-baby")

const selectTypeBackbutton = document.querySelector(".search-type-backbutton");
const selectCategoryBackbutton = document.querySelectorAll(".select-category-backbutton");

const selectedCategoryInfo = document.querySelector(".selected-category-info");

let selectedCategory;

SetupMenu();

function HideAll() {
	collectionSelectionWomen.style.display = 'none';
	collectionSelectionMen.style.display = 'none';
	collectionSelectionKids.style.display = 'none';
	collectionSelectionBaby.style.display = 'none';

	subcategorySelectionWomen.style.display = 'none';
	subcategorySelectionMen.style.display = 'none';
	subcategorySelectionKids.style.display = 'none';
	subcategorySelectionBaby.style.display = 'none';

	searchTypeSelection.style.display = 'none';

	categorySelection.style.display = 'none';
}

function SetupMenu() {
	searchTypeSelection.style.display = 'none';
}

function ToSearchType() {
	categorySelection.style.display = 'none';
	searchTypeSelection.style.display = 'block';
	selectedCategoryInfo.textContent = selectedCategory;
}

selectWomenLink.addEventListener('click', function handleClickEvent() {
	selectedCategory = $("#WomenCategoryName").val();
	ToSearchType();
})
selectMenLink.addEventListener('click', function handleClickEvent() {
	selectedCategory = $("#MenCategoryName").val();
	ToSearchType();
})
selectKidsLink.addEventListener('click', function handleClickEvent() {
	selectedCategory = $("#KidsCategoryName").val();
	ToSearchType();
})
selectBabyLink.addEventListener('click', function handleClickEvent() {
	selectedCategory = $("#BabyCategoryName").val();
	ToSearchType();
})
selectCategoryBackbutton.forEach(element =>
	element.addEventListener('click', function handleClickEvent() {
		HideAll();
		searchTypeSelection.style.display = 'block';
	})
)
selectTypeBackbutton.addEventListener('click', function handleClickEvent() {
	HideAll();
	categorySelection.style.display = 'block';
})

selectByCollectionLink.addEventListener('click', function handleClickEvent() {
	searchTypeSelection.style.display = 'none';
	categorySelection.style.display = 'none';
	if (selectedCategory == $("#WomenCategoryName").val()) {
		collectionSelectionWomen.style.display = 'block';
	}
	else if (selectedCategory == $("#MenCategoryName").val()) {
		collectionSelectionMen.style.display = 'block';
	}
	else if (selectedCategory == $("#KidsCategoryName").val()) {
		collectionSelectionKids.style.display = 'block';
	}
	else {
		collectionSelectionBaby.style.display = 'block';
	}
})
selectByCategory.addEventListener('click', function handleClickEvent() {
	searchTypeSelection.style.display = 'none';
	categorySelection.style.display = 'none';
	if (selectedCategory == $("#WomenCategoryName").val()) {
		subcategorySelectionWomen.style.display = 'block';
	}
	else if (selectedCategory == $("#MenCategoryName").val()) {
		subcategorySelectionMen.style.display = 'block';
	}
	else if (selectedCategory == $("#KidsCategoryName").val()) {
		subcategorySelectionKids.style.display = 'block';
	}
	else {
		subcategorySelectionBaby.style.display = 'block';
	}
})

menuButton.onclick = ClickMenu;

function ClickMenu() {
	let isActive = mobileMenu.style.display == 'block';
	if (isActive) {
		mobileMenu.style.display = 'none';
	}
	else {
		mobileMenu.style.display = 'block';
	}
}
window.onresize = function () {
	if (window.innerWidth > 1100) {
		mobileMenu.style.display = 'none';
	}
}