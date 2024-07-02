// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


let nav = document.getElementsByClassName('navbar-toggler');
let offcan = document.getElementsByClassName('offcanvas-backdrop');
nav[0].addEventListener('click', function () {
    let icon = this.children;
    let closed = icon[0].checkVisibility();

    if (closed) {
        icon[1].classList.remove('visually-hidden');
        icon[0].classList.add('visually-hidden');
    } 
    offcan[0].addEventListener('click', function () {
        
        icon[0].classList.remove('visually-hidden');
        icon[1].classList.add('visually-hidden');
        
    });

});
