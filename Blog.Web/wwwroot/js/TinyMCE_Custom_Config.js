tinymce.init({
    selector: 'textarea#tiny',
    plugins: [
        'advlist', 'autolink',
        'lists', 'link', 'image', 'charmap', 'preview', 'anchor', 'searchreplace', 'visualblocks',
        'fullscreen', 'insertdatetime', 'media', 'table', 'help', 'wordcount'
    ],
    toolbar: 'undo redo | a11ycheck casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify |' +
        'bullist numlist checklist outdent indent | removeformat | code table help',
    image_title: true,
    automatic_uploads: true,
    file_picker_callback: (callback, value, meta) => {
        if (meta.filetype == 'image') {
            var input = document.createElement('input');
            input.setAttribute('type', 'file');
            input.setAttribute('accept', 'image/*');
            input.setAttribute('multiple', 'multiple'); // Allow multiple file selection

            input.onchange = function () {
                var files = this.files;
                var formData = new FormData();

                for (var i = 0; i < files.length; i++) {
                    formData.append('files', files[i]);
                }

                fetch('/upload', {
                    method: 'POST',
                    body: formData
                })
                    .then(response => response.json())
                    .then(data => {
                        data.locations.forEach(function (location) {
                            callback(location, { alt: 'Image' });
                        });
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            };

            input.click();
        }
    },
})