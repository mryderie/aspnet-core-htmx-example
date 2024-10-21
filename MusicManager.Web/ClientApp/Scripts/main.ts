import "../styles/stylesVendor.ts";
import "../styles/main.scss";

// need this line to ensure that htmx is included in the bundle even when it's not directly used
import "htmx.org"
import * as htmx from "htmx.org"
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



export function closeModal(modalId: string, callback?: Function) {

    const modelEl = document.getElementById(modalId);
    const modal = Modal.getInstance(modelEl);
    modal.hide();

    if (callback) {
        modelEl.addEventListener('hidden.bs.modal', event => {
            callback();
        });
    }
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
    // NOTE: this event can fire multiple times for a single request
    // Add the id of the item to the URL as a param - easier than trying to dynamically update the form submit URL after HTMX has initialised it.
    if (e.detail.verb === "delete") {
        if (e.detail.parameters.deleteItemId) {
            const deleteUrl = new URL(e.detail.path, location.origin);
            if (!deleteUrl.searchParams.get('id')) {
                deleteUrl.searchParams.set('id', e.detail.parameters.deleteItemId);
            }

            e.detail.path = deleteUrl.pathname + deleteUrl.search;
        }
    }
});

document.body.addEventListener("gridItemEdit", function () {
    closeModal("itemModalContainer", () => htmx.ajax("GET", document.location.href));
});

document.body.addEventListener("gridItemDelete", function () {
    closeModal("confirmDeleteModalContainer", () => htmx.ajax("GET", document.location.href));
});

export function isFormValid(submitter: HTMLElement) {
    const form = submitter.closest("form");

    // prevent double submission
    if ("submitted" in form.dataset) {
        return false;
    }
    else {
        // first submission - check if valid
        const isValid = v.isValid(form);

        if (isValid) {
            // mark the form as submitted
            form.dataset.submitted = "submitted";
        }

        return isValid;
    }
}

export function preventMultiSubmit(form: HTMLFormElement) {
    if ("submitted" in form.dataset) {
        // already submitted
        return false;
    }
    else {
        // first submission
        form.dataset.submitted = "submitted";
        return true;
    }
}

export function confirmDelete(id: string, itemName: string) {
    // set the values in the Deletion modal
    document.getElementById("deleteItemName").textContent = itemName;
    document.getElementById("deleteItemId").setAttribute("value", id);
}