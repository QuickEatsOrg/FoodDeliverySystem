// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("click", function (event) {
    const interactiveElement = event.target.closest("a, button, input, form, label, select, textarea");
    if (interactiveElement) {
        return;
    }

    const card = event.target.closest(".menu-card[data-details-url]");
    if (card) {
        window.location.href = card.dataset.detailsUrl;
    }
});

document.addEventListener("keydown", function (event) {
    if (event.key !== "Enter" && event.key !== " ") {
        return;
    }

    const card = event.target.closest(".menu-card[data-details-url]");
    if (card) {
        event.preventDefault();
        window.location.href = card.dataset.detailsUrl;
    }
});
