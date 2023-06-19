let mainSlider = document.querySelector("#main-splide");
let sideSlider = document.querySelector("#side-splide");

document.addEventListener("DOMContentLoaded", function () {
  let main = new Splide("#main-splide", {
    type: "fade",
    rewind: true,
    pagination: false,
    arrows: false,
  });
  let thumbnails = new Splide("#side-splide", {
    height: "100%",
    direction: "ttb",
    perPage: 4,
    rewind: true,
    isNavigation: true,
    gap: 10,
    pagination: false,
    cover: true,
    wheel: true,
    wheelMinThreshold: 10,
    dragMinThreshold: {
      mouse: 4,
      touch: 10,
    },
  });
  main.sync(thumbnails);
  main.mount();
  thumbnails.mount();

  if (window.innerWidth < 1000) {
    MobileFormation();
  } else {
    DesktopFormation();
  }

  window.onresize = function () {
    if (window.innerWidth < 1000) {
      MobileFormation();
    } else {
      DesktopFormation();
    }
  };
});

function DesktopFormation() {
  sideSlider.style.display = "block";
  main = new Splide("#main-splide", {
    type: "fade",
    width: "700px",
    rewind: true,
    pagination: false,
    arrows: false,
  });
  main.mount();
}

function MobileFormation() {
  sideSlider.style.display = "none";
  main = new Splide("#main-splide", {
    type: "fade",
    width: "100%",
    rewind: true,
    pagination: false,
    arrows: true,
  });
  main.mount();
}
