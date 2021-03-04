// Наступний код сортує таблицю з класом .table-sort при натисканні на відповідний заголовок.

document.addEventListener('DOMContentLoaded', () => {
    const getSort = ({ target }) => {
        if (target.innerText.length) {
            const order = (target.dataset.order = target.dataset.order || -1);
            const index = [...target.parentNode.cells].indexOf(target);
            const collator = new Intl.Collator(['en', 'ru'], { numeric: true });
            const comparator = (index, order) => (a, b) => order * collator.compare(
                a.children[index].innerHTML,
                b.children[index].innerHTML
            );

            for (const tBody of target.closest('table').tBodies)
                tBody.append(...[...tBody.rows].sort(comparator(index, order)));

            for (const cell of target.parentNode.cells)
                cell.classList.toggle('sorted', cell === target);
        };
    };

    document.querySelectorAll('.table-sort thead').forEach(tableTH => tableTH.addEventListener('click', () => getSort(event)));

});



//  Табличне розташування елементів.

let gutter = parseInt(jQuery('.post').css('marginBottom'));
let container = jQuery('#posts').first();

//  При зміні розмірів вікна наступний код змінює розміщення та розміри елементів сітки з класом .post, 
//  які не є дочірніми від класса .container.

jQuery(window).bind('resize', function () {
    if (!jQuery('#posts').parent().hasClass('container')) {

        post_width = jQuery('.post').width() + gutter;
        jQuery('#posts, body > #grid').css('width', 'auto');

        posts_per_row = jQuery('#posts').innerWidth() / post_width;
        floor_posts_width = (Math.floor(posts_per_row) * post_width) - gutter;
        ceil_posts_width = (Math.ceil(posts_per_row) * post_width) - gutter;
        posts_width = (ceil_posts_width > jQuery('#posts').innerWidth()) ? floor_posts_width : ceil_posts_width;
        if (posts_width == jQuery('.post').width()) {
            posts_width = '100%';
        }

        //  Встановлює однакову для всіх елементів верхньго слою ширину, та розміщує їх по центру.

        jQuery('#posts, #grid').css('width', posts_width);
        jQuery('#grid').css({
            'margin': '0 auto'
        });

    }
}).trigger('resize');



//  Додає елемент списку, який містить поле тільки для читання, значення якого є назва жанру.

function addGenre(selectBox, name) {
    if (!document.querySelector(`#genreList input[value="${selectBox.value}"]`)) {
        let copy = addTextBox($(genreList), name);
        $(copy).children("input").attr("value", selectBox.value);
    }
    $(selectBox).val("");
}

//  Додає елемент списку, який містить поле для вводу тексту.

function addTextBox(parentList, name) {
    let li = $(parentList).children().last();
    let copy = $(li).clone();
    let input = $(copy).children("input");
    $(copy).attr("hidden", false);
    $(input).attr("name", name);
    $(li).before(copy);
    return copy;
}

//  Встановлює параметри маршруту: id елемента для видалення, назва методу, який буде обробляти запрос,
//  назва параметру метода, який приймає значення id.

function setRemoveId(id, handler = "ByRemove", paramName = "id") {
    $(removeBtn).attr('formaction', `?${paramName}=${id}&handler=${handler}`);
}

//  Встановлює початкові параметри для редагування жанру.

function setStartParams(id, name, summary) {
    $(modalBoxTitle).text("Редагувати жанр");
    $(Genre_Name).val(name);
    $(Genre_Summary).val(summary);
    $(Genre_Id).val(id);
}

//  Встановлює початкові параметри для додавання жанру.

function setDefaultParams() {
    $(modalBoxTitle).text("Додати жанр");
    $(Genre_Name).val(null);
    $(Genre_Summary).val(null);
}

//  Встановлює параметри для видалення окремого фільму з колекції.

function setMovieRemoveParams(movieId) {
    setRemoveId(movieId, "ByRemoveMovie", "movieId");
    $("#removeConfirm form").append(currentCollection);
}




