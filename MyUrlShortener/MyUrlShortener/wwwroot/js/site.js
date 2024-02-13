'use strict';
(($) => {
    $(function () {

        const userUrlField = $('#userUrl');

        function getDTO() {
            let dto = {
                userUrl: userUrlField.val(),
            }
            return dto;
        }

        const btnGetShotUrl = $('#btnGetShotUrl');
        const convertToShortURL = $('#convertToShort').val();
        const showShortUrl = document.getElementById('showShortUrl');
        const shortUrl = document.getElementById('shortUrl');
        btnGetShotUrl.on('click', (() => {
            let dto = getDTO();
            btnGetShotUrl.html('Ожидайте');
            btnGetShotUrl.prop('disabled', true);
            $.post(convertToShortURL, dto)
                .then((response) => {
                    btnGetShotUrl.html('Сократить');
                    btnGetShotUrl.prop('disabled', false);
                    showShortUrl.removeAttribute("hidden");
                    if (response.includes("Извините")) {
                        shortUrl.innerHTML = response;
                    }
                    else {
                        shortUrl.innerHTML = "<a href = \"" + response + "\">" + window.location.href + response + "</a>";
                    }
                    
                });
        }));
        

    });
})(jQuery);
