import "../styles/stylesVendor.ts";
import "../styles/main.scss";

// need this line to ensure that htmx is included in the bundle even when it's not directly used
import "htmx.org"
import htmx from "htmx.org"
import { HtmxRequestConfig } from "htmx.org"
import * as nprogress from "nprogress";
import { Modal } from "bootstrap";
import { Collapse } from "bootstrap";
import { ValidationService } from "aspnet-client-validation";

const v = new ValidationService();
v.ValidationInputCssClassName = 'is-invalid';                 // change from default of 'input-validation-error'
v.ValidationInputValidCssClassName = 'is-valid';                   // change from default of 'input-validation-valid'
v.ValidationMessageCssClassName = 'invalid-feedback';           // change from default of 'field-validation-error'
v.ValidationMessageValidCssClassName = 'valid-feedback';             // change from default of 'field-validation-valid'
v.bootstrap({ watch: true });



function closeModalAndRefreshBody(modalId: string) {

    const modalEl = document.getElementById(modalId);
    // Refresh the page body after the modal close animation completes
    modalEl.addEventListener("hidden.bs.modal", () => htmx.ajax("get", document.location.href, undefined));

    const modal = Modal.getInstance(modalEl);
    modal.hide();
}

function removeItemModal() {
    const modalEl = document.getElementById("itemModalContainer");
    // remove modal content completely so that it does not briefly appear the next time a modal is opened
    modalEl.querySelector(".modal-content")?.remove();
}
function errorRefresh(details: any) {
    //todo - log details

    alert("Sorry, an error occurred. The page will now refresh. Please try again.");
    location.reload();
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
    // Add the anti-forgery token from the form to the request headers
    const requestConfig = e.detail as HtmxRequestConfig;
    if (requestConfig?.unfilteredFormData
        && !requestConfig.headers["RequestVerificationToken"]) {
        const token = requestConfig.unfilteredFormData.get("__RequestVerificationToken");
        requestConfig.headers["RequestVerificationToken"] = token as string;
    }
});

// "showItemModal" is set in the "HX-Trigger-After-Settle" header by the server in a modal response
document.body.addEventListener("showItemModal", function () {
    // remove the backdrop that was shown while waiting for the server response
    const modalLoadingBackdrop = document.getElementById("modalLoadingBackdrop");
    modalLoadingBackdrop.classList.replace("show", "hide");
    setTimeout(function () {
        // wait 500ms for the "hide" fade-out animation to complete, then remove the loaing backdrop element
        // todo - there is a slight flicker as the loading backdrop fades out, and the new backdrop fades in
        modalLoadingBackdrop?.remove();
    }, 500);

    // show the new modal that was provided by the server
    const modalEl = document.getElementById("itemModalContainer");
    modalEl.addEventListener('hidden.bs.modal', removeItemModal);
    const modal = Modal.getOrCreateInstance(modalEl);
    modal.show();
});

document.body.addEventListener("gridItemEdit", function () {
    closeModalAndRefreshBody("itemModalContainer");
});

document.body.addEventListener("gridItemDelete", function () {
    closeModalAndRefreshBody("confirmDeleteModalContainer");
});

export function showModalBackdrop() {
    // show a transparent modal backdrop while waiting for a server modal response
    const backdrop = document.createElement("div");
    backdrop.setAttribute("id", "modalLoadingBackdrop")
    backdrop.className = "modal-backdrop fade show";
    document.body.appendChild(backdrop);
}

export function isFormValid(submitter: HTMLElement) {
    const form = submitter.closest("form");
    return v.isValid(form);
}

export function confirmDelete(id: string, itemName: string) {
    // set the values in the Deletion modal
    document.getElementById("deleteItemName").textContent = itemName;
    document.getElementById("deleteItemId").setAttribute("value", id);
}