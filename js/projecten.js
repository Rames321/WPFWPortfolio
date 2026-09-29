const maakTechLijst = (technieken) => {
    const lijst = document.createElement('ul');
    lijst.className = 'tech';

    technieken.forEach((techniek) => {
        const item = document.createElement('li');
        item.textContent = techniek;
        lijst.appendChild(item);
    });

    return lijst;
};

const maakLink = (url, tekst) => {
    const link = document.createElement('a');
    link.href = url;
    link.textContent = tekst;
    return link;
};

const maakProjectKaart = (project) => {
    const artikel = document.createElement('article');

    const titel = document.createElement('h3');
    titel.textContent = project.titel;

    const afbeelding = document.createElement('img');
    afbeelding.src = project.afbeelding.src;
    afbeelding.width = project.afbeelding.breedte;
    afbeelding.height = project.afbeelding.hoogte;
    afbeelding.alt = project.afbeelding.alt;

    const beschrijving = document.createElement('p');
    beschrijving.textContent = project.beschrijving;

    const links = document.createElement('p');
    links.className = 'links';
    links.appendChild(maakLink(project.demo, 'Live demo'));
    if (project.github) {
        links.appendChild(maakLink(project.github, 'Broncode op GitHub'));
    }

    artikel.append(titel, afbeelding, beschrijving, maakTechLijst(project.tech), links);
    return artikel;
};

const renderProjecten = (lijst, container) => {
    container.replaceChildren();

    lijst.forEach((project) => {
        container.appendChild(maakProjectKaart(project));
    });
};

const uniekeTechnieken = (lijst) => {
    const alle = lijst.flatMap((project) => project.tech);
    return [...new Set(alle)].sort((a, b) => a.localeCompare(b, 'nl'));
};

const filterOpTech = (lijst, techniek) => {
    if (techniek === 'Alle') {
        return lijst;
    }
    return lijst.filter((project) => project.tech.includes(techniek));
};

const toonAantal = (element, aantal, totaal) => {
    element.textContent = `${aantal} van ${totaal} projecten`;
};

const markeerKnop = (techniek) => {
    document.querySelectorAll('#filter-knoppen button').forEach((knop) => {
        knop.setAttribute('aria-pressed', knop.textContent === techniek);
    });
};

const kiesFilter = (techniek) => {
    const gefilterd = filterOpTech(projecten, techniek);
    renderProjecten(gefilterd, document.querySelector('#projecten-lijst'));
    toonAantal(document.querySelector('#projecten-aantal'), gefilterd.length, projecten.length);
    markeerKnop(techniek);
};

const maakFilterKnoppen = (container, technieken) => {
    ['Alle', ...technieken].forEach((techniek) => {
        const knop = document.createElement('button');
        knop.type = 'button';
        knop.textContent = techniek;
        knop.addEventListener('click', () => kiesFilter(techniek));
        container.appendChild(knop);
    });
};

const startProjecten = () => {
    maakFilterKnoppen(document.querySelector('#filter-knoppen'), uniekeTechnieken(projecten));
    kiesFilter('Alle');
};

startProjecten();
