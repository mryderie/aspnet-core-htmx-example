import "../styles/stylesmain.ts";

import * as $ from "jquery";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "bootstrap/js/src/modal.js";
import "bootstrap/js/src/collapse.js";
import * as htmx from "htmx.org/dist/htmx.js";
import * as nprogress from "nprogress/nprogress.js";


// this will be called when new content is loaded in the browser
htmx.onLoad(function (target: HTMLElement) {

    const forms = target.getElementsByTagName("form");
    for (let i = 0; i < forms.length; i++) {
        // add validation
        $.validator.unobtrusive.parse(forms[i]);
    }
});

export function closeItemModal(callback?: Function) {

    const container = document.getElementById("itemModalContainer");
    const modal = document.getElementById("itemModal");
    const backdrop = document.getElementById("itemModalBackdrop");

    if (modal)
        modal.classList.remove("show");
    if (backdrop)
        backdrop.classList.remove("show");

    setTimeout(function () {

        if (backdrop)
            container.removeChild(backdrop);
        if (modal)
            container.removeChild(modal);

        if (callback)
            callback();
    }, 200);
}

function closeConfirmDeleteModal(callback?: Function) {
    $("#confirmDeleteModal").modal("hide");

    if (callback) {
        setTimeout(function () {
            callback();
        }, 200);
    }
}

function errorRefresh(details: any) {
    //todo - log details

    alert("Sorry, an error occurred. The page will now refresh. Please try again.");
    location.reload(true);
}

document.body.addEventListener("htmx:responseError", function (e: any) {
    errorRefresh(e.details);
});

document.body.addEventListener("htmx:onLoadError", function (e: any) {
    errorRefresh(e.details);
});

document.body.addEventListener("htmx:xhr:loadstart", function (e: any) {
    nprogress.start();
});

document.body.addEventListener("htmx:xhr:progress", function (e: any) {
    nprogress.inc();
});

document.body.addEventListener("htmx:afterSettle", function (e: any) {
    // this event seems to occur more reliably than htmx:xhr:loadend
    nprogress.done();
});

document.body.addEventListener('htmx:configRequest', function (e: any) {
    // Append the id of the item to the URL so that it's consistent with other URL formats, and more REST-ful :)
    // Easier than trying to dynamically update the form submit URL after HTMX has initialised it.
    if (e.detail.verb === "delete") {
        if (e.detail.parameters.deleteItemId) {
            e.detail.path = e.detail.path + "/" + e.detail.parameters.deleteItemId;
        }
    }
});

document.body.addEventListener("gridItemEdit", function () {
    closeItemModal(() => htmx.ajax("GET", document.location.href));
});

document.body.addEventListener("gridItemDelete", function () {
    closeConfirmDeleteModal(() => htmx.ajax("GET", document.location.href));
});

export function isFormValid(submitter: HTMLElement) {

    const form = $(submitter).closest("form");

    // prevent double submission
    if ($(form).data("submitted")) {
        return false;
    }
    else {
        // first submission - check if valid
        const isValid = form.valid();

        if (isValid)
            $(form).data("submitted", true);

        return isValid;
    }
}

export function preventMultiSubmit(form: HTMLFormElement) {
    if ($(form).data("submitted")) {
        // already submitted
        return false;
    }
    else {
        // first submission
        $(form).data("submitted", true);
        return true;
    }
}

export function confirmDelete(id: number, itemName: string) {

    // set the values in the Deletion modal
    $("#deleteItemName").text(itemName);
    $("#deleteItemId").attr("value", id);

    // show the deletion modal
    $("#confirmDeleteModal").modal();
}