const GITHUB_URL = 'https://api.github.com/users/Rames321/repos?sort=pushed&per_page=5';

const haalRepos = async () => {
    const antwoord = await fetch(GITHUB_URL);
    if (!antwoord.ok) {
        throw new Error(`GitHub gaf status ${antwoord.status}`);
    }
    return antwoord.json();
};

const formatteerDatum = (isoDatum) => {
    return new Date(isoDatum).toLocaleDateString('nl-NL', { day: 'numeric', month: 'long', year: 'numeric' });
};

const maakRepoItem = (repo) => {
    const item = document.createElement('li');

    const link = document.createElement('a');
    link.href = repo.html_url;
    link.textContent = repo.name;

    const info = document.createElement('p');
    info.className = 'meta';
    info.textContent = `${repo.language ?? 'Taal onbekend'} · bijgewerkt op ${formatteerDatum(repo.pushed_at)}`;

    item.append(link, info);
    return item;
};

const toonRepos = (repos, lijst) => {
    repos.forEach((repo) => {
        lijst.appendChild(maakRepoItem(repo));
    });
};

const startGithub = async () => {
    const status = document.querySelector('#github-status');
    const lijst = document.querySelector('#github-lijst');

    status.textContent = 'Repositories laden...';

    try {
        const repos = await haalRepos();
        toonRepos(repos, lijst);
        status.textContent = repos.length === 0 ? 'Nog geen openbare repositories.' : '';
    } catch (fout) {
        status.textContent = 'GitHub is nu niet bereikbaar. Probeer het later opnieuw.';
        status.classList.add('fout');
    }
};

startGithub();
