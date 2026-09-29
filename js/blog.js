const wisselPost = (knop, tekst) => {
    const isOpen = knop.getAttribute('aria-expanded') === 'true';
    knop.setAttribute('aria-expanded', !isOpen);
    tekst.hidden = isOpen;
};

const maakUitklapKnop = (titel, tekst) => {
    const knop = document.createElement('button');
    knop.type = 'button';
    knop.textContent = titel;
    knop.setAttribute('aria-expanded', 'false');
    knop.setAttribute('aria-controls', tekst.id);
    knop.addEventListener('click', () => wisselPost(knop, tekst));
    return knop;
};

const maakPostUitklapbaar = (post, index) => {
    const kop = post.querySelector('h3');
    const tekst = post.querySelector('.blog-tekst');

    tekst.id = `blog-tekst-${index + 1}`;
    tekst.hidden = true;
    kop.replaceChildren(maakUitklapKnop(kop.textContent, tekst));
};

const startBlog = () => {
    document.querySelectorAll('article').forEach(maakPostUitklapbaar);
};

startBlog();
