window.moonlight = {
    toasts: {
        success: function (title, message, timeout) {
            this.show(title, message, timeout, "success");
        },
        danger: function (title, message, timeout) {
            this.show(title, message, timeout, "danger");
        },
        warning: function (title, message, timeout) {
            this.show(title, message, timeout, "warning");
        },
        info: function (title, message, timeout) {
            this.show(title, message, timeout, "info");
        },
        show: function (title, message, timeout, color) {
            var toast = new ToastHelper(title, message, color, timeout);
            toast.show();
        },

        // Progress toasts 
        progress: {
            create: function (id, text) {
                var toast = new ToastHelper("Progress", text, "secondary", 0);
                toast.showAlways();

                toast.domElement.setAttribute('data-ml-toast-id', id);
            },
            modify: function (id, text) {
                var toast = document.querySelector('[data-ml-toast-id="' + id + '"]');

                toast.getElementsByClassName("toast-body")[0].innerText = text;
            },
            remove: function (id) {
                var toast = document.querySelector('[data-ml-toast-id="' + id + '"]');
                bootstrap.Toast.getInstance(toast).hide();

                setTimeout(() => {
                    toast.remove();
                }, 2);
            }
        }
    },
    modals: {
        show: function (id, focus) {
            let modal = new bootstrap.Modal(document.getElementById(id), {
                focus: focus
            });

            modal.show();
        },
        hide: function (id) {
            let element = document.getElementById(id)
            let modal = bootstrap.Modal.getInstance(element)
            modal.hide()
        }
    },
    alerts: {
        getHelper: function () {
            return Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success',
                    cancelButton: 'btn btn-danger',
                    denyButton: 'btn btn-danger',
                    htmlContainer: 'text-white'
                },
                buttonsStyling: false
            });
        },
        info: function (title, description) {
            this.getHelper().fire(
                title,
                description,
                'info'
            )
        },
        success: function (title, description) {
            this.getHelper().fire(
                title,
                description,
                'success'
            )
        },
        warning: function (title, description) {
            this.getHelper().fire(
                title,
                description,
                'warning'
            )
        },
        error: function (title, description) {
            this.getHelper().fire(
                title,
                description,
                'error'
            )
        },
        yesno: function (title, yesText, noText) {
            return this.getHelper().fire({
                title: title,
                showDenyButton: true,
                confirmButtonText: yesText,
                denyButtonText: noText,
            }).then((result) => {
                if (result.isConfirmed) {
                    return true;
                } else if (result.isDenied) {
                    return false;
                }
            })
        },
        text: function (title, description) {
            const {value: text} = this.getHelper().fire({
                title: title,
                input: 'text',
                inputLabel: description,
                inputValue: "",
                showCancelButton: false,
                inputValidator: (value) => {
                    if (!value) {
                        return 'You need to enter a value'
                    }
                }
            })

            return text;
        }
    },
    textEditor: {
        create: function (id) {
            BalloonEditor
                .create(document.getElementById(id), {
                    toolbar: ['heading', '|', 'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote'],
                    heading: {
                        options: [
                            {model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph'},
                            {model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1'},
                            {model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2'}
                        ]
                    }
                })
                .catch(error => {
                    console.error(error);
                });
        },
        get: function (id) {
            let editor = document.getElementById(id).ckeditorInstance;
            return editor.getData();
        },
        set: function (id, data) {
            let editor = document.getElementById(id).ckeditorInstance;
            editor.setData(data);
        }
    },
    utils: {
        download: async function (fileName, contentStreamReference) {
            const arrayBuffer = await contentStreamReference.arrayBuffer();
            const blob = new Blob([arrayBuffer]);
            const url = URL.createObjectURL(blob);
            const anchorElement = document.createElement('a');
            anchorElement.href = url;
            anchorElement.download = fileName ?? '';
            anchorElement.click();
            anchorElement.remove();
            URL.revokeObjectURL(url);
        }
    },
    clipboard: {
        copy: function (text) {
            if (!navigator.clipboard) {
                var textArea = document.createElement("textarea");
                textArea.value = text;

                // Avoid scrolling to bottom
                textArea.style.top = "0";
                textArea.style.left = "0";
                textArea.style.position = "fixed";

                document.body.appendChild(textArea);
                textArea.focus();
                textArea.select();

                try {
                    var successful = document.execCommand('copy');
                    var msg = successful ? 'successful' : 'unsuccessful';
                } catch (err) {
                    console.error('Fallback: Oops, unable to copy', err);
                }

                document.body.removeChild(textArea);
                return;
            }
            navigator.clipboard.writeText(text).then(function () {
                },
                function (err) {
                    console.error('Async: Could not copy text: ', err);
                }
            );
        }
    },
    dropzone: {
        create: function (elementId, url) {
            var id = "#" + elementId;
            var dropzone = document.querySelector(id);

            // set the preview element template
            var previewNode = dropzone.querySelector(".dropzone-item");
            previewNode.id = "";
            var previewTemplate = previewNode.parentNode.innerHTML;
            previewNode.parentNode.removeChild(previewNode);

            var fileDropzone = new Dropzone(id, {
                url: url,
                maxFilesize: 100,
                previewTemplate: previewTemplate,
                previewsContainer: id + " .dropzone-items",
                clickable: ".dropzone-panel",
                createImageThumbnails: false,
                ignoreHiddenFiles: false,
                disablePreviews: false
            });

            fileDropzone.on("addedfile", function (file) {
                const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
                dropzoneItems.forEach(dropzoneItem => {
                    dropzoneItem.style.display = '';
                });

                // Create a progress bar for the current file
                var progressBar = dropzone.querySelector('.dropzone-item .progress-bar');
                progressBar.style.width = "0%";
            });

// Update the progress bar for each file
            fileDropzone.on("uploadprogress", function (file, progress, bytesSent) {
                var dropzoneItem = file.previewElement;
                var progressBar = dropzoneItem.querySelector('.progress-bar');
                progressBar.style.width = progress + "%";
            });

// Hide the progress bar for each file when the upload is complete
            fileDropzone.on("complete", function (file) {
                var dropzoneItem = file.previewElement;
                var progressBar = dropzoneItem.querySelector('.progress-bar');

                setTimeout(function () {
                    progressBar.style.opacity = "1";
                    progressBar.classList.remove("bg-primary");
                    progressBar.classList.add("bg-success");
                }, 300);
            });
        },
        updateUrl: function (elementId, url) {
            Dropzone.forElement("#" + elementId).options.url = url;
        }
    }
}