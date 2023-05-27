let isSomeOpene = false;
let isLinkHovered = false;
let isDivHovered = false;
let timerStarted = false;
let canShowDiv = false;
let accountActionsIsOpen = false;
let totalUnderlinedElement;
let totalShowDiv;

document.addEventListener('DOMContentLoaded', function () {
	const accountActions = document.querySelector("#account-actions");
	const openAccountActions = document.querySelector("#open-account-actions");

	const womenDropMenu = document.querySelector("#Women-drop-menu");
	const menDropMenu = document.querySelector("#Men-drop-menu");
	const kidsDropMenu = document.querySelector("#Kids-drop-menu");
	const babyDropMenu = document.querySelector("#Baby-drop-menu");

	const categoryLinkWomen = document.querySelector("#category-women");
	const categoryLinkMen = document.querySelector("#category-men");
	const categoryLinkKids = document.querySelector("#category-kids");
	const categoryLinkBaby = document.querySelector("#category-baby");

	const hovers = document.querySelectorAll(".main-menu-category-link");
	const hiddenDiv = document.querySelectorAll(".drop-main-menu");

	const mobileSearch = document.querySelector(".mobile-search")
	const mobileSearchCloseButton = document.querySelector(".close-mobile-search-button")
	const mobileSearchButton = document.querySelector(".mobile-search-button")


	if (openAccountActions != null) {
		openAccountActions.addEventListener('click', function () {
			if (accountActionsIsOpen == false) {
				accountActions.style.display = 'block';
				accountActionsIsOpen = true;
			}
			else {
				accountActions.style.display = 'none';
				accountActionsIsOpen = false;
			}
		})
	}

	mobileSearchButton.addEventListener('click', function () {
		mobileSearch.style.display = 'block';
	})
	mobileSearchCloseButton.addEventListener('click', function () {
		mobileSearch.style.display = 'none';
	})

	categoryLinkWomen.addEventListener('mouseover', function handleMouseOverEvent() {
		if (ShowDiv(womenDropMenu)) {
			if (totalUnderlinedElement != null) {
				totalUnderlinedElement.firstChild.style.borderStyle = "none";
			}
			categoryLinkWomen.firstChild.style.borderStyle = "solid";
			totalUnderlinedElement = categoryLinkWomen;
		}
	})
	categoryLinkMen.addEventListener('mouseover', function handleMouseOverEvent() {
		if (ShowDiv(menDropMenu)) {
			if (totalUnderlinedElement != null) {
				totalUnderlinedElement.firstChild.style.borderStyle = "none";
			}
			categoryLinkMen.firstChild.style.borderStyle = "solid";
			totalUnderlinedElement = categoryLinkMen;
		}
	})
	categoryLinkKids.addEventListener('mouseover', function handleMouseOverEvent() {
		if (ShowDiv(kidsDropMenu)) {
			if (totalUnderlinedElement != null) {
				totalUnderlinedElement.firstChild.style.borderStyle = "none";
			}
			categoryLinkKids.firstChild.style.borderStyle = "solid";
			totalUnderlinedElement = categoryLinkKids;
		}
	})
	categoryLinkBaby.addEventListener('mouseover', function handleMouseOverEvent() {
		if (ShowDiv(babyDropMenu)) {
			if (totalUnderlinedElement != null) {
				totalUnderlinedElement.firstChild.style.borderStyle = "none";
			}
			categoryLinkBaby.firstChild.style.borderStyle = "solid";
			totalUnderlinedElement = categoryLinkBaby;
		}
	})
	for (index = 0; index < hovers.length; ++index) {

		hovers[index].addEventListener('mouseout', function handleMouseOutEvent() {
			isLinkHovered = false;
			SetTimerToHideDivs(200);
		})
		hovers[index].addEventListener('mouseover', function handleMouseOverEvent() {
			isLinkHovered = true;
		})

		hiddenDiv[index].addEventListener('mouseover', function handleMouseOverEvent() {
			isDivHovered = true;
		})
		hiddenDiv[index].addEventListener('mouseout', function handleMouseOutEvent() {
			isDivHovered = false;
			SetTimerToHideDivs(10)
		})
	}

	window.onload = function () {
		ShowTimeout()
	}

	async function SetTimerToHideDivs(time) {
		isLinkHovered = false;
		if (timerStarted == false) {
			timerStarted = true;
			await new Promise(resolve => setTimeout(resolve, time));
			HideAllDivs();
		}
	}

	function HideAllDivs() {
		timerStarted = false;
		if (!isDivHovered && !isLinkHovered) {
			womenDropMenu.style.display = 'none';
			menDropMenu.style.display = 'none';
			kidsDropMenu.style.display = 'none';
			babyDropMenu.style.display = 'none';
			if (totalUnderlinedElement != null) {
				totalUnderlinedElement.firstChild.style.borderStyle = "none";
			}
			isSomeOpene = false;
		}
	}

	async function ShowDiv(divToShow) {
		if (canShowDiv) {
			if (totalShowDiv != null) {
				totalShowDiv.style.display = 'none';
			}
			divToShow.style.display = 'block';
			totalShowDiv = divToShow;
			isSomeOpene = true;
			return true;
		}
		return false;
	}

	async function ShowTimeout() {
		await new Promise(resolve => setTimeout(resolve, 250));
		canShowDiv = true;
	}
});
