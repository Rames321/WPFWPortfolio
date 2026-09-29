const EMAIL_PATROON = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const MIN_BERICHT = 20;

const controleerNaam = (waarde) => {
    const naam = waarde.trim();
    if (naam === '') {
        return 'Vul je naam in.';
    }
    if (naam.length < 2) {
        return 'Je naam moet minstens 2 tekens hebben.';
    }
    return '';
};

const controleerEmail = (waarde) => {
    const email = waarde.trim();
    if (email === '') {
        return 'Vul je e-mailadres in.';
    }
    if (!EMAIL_PATROON.test(email)) {
        return 'Dit e-mailadres klopt niet. Gebruik de vorm naam@voorbeeld.nl.';
    }
    return '';
};

const controleerBericht = (waarde) => {
    const lengte = waarde.trim().length;
    if (lengte === 0) {
        return 'Schrijf een bericht.';
    }
    if (lengte < MIN_BERICHT) {
        return `Je bericht is te kort: minimaal ${MIN_BERICHT} tekens, je hebt er nu ${lengte}.`;
    }
    return '';
};

const controles = {
    naam: controleerNaam,
    email: controleerEmail,
    bericht: controleerBericht
};

const toonFout = (veld, melding) => {
    document.querySelector(`#${veld.id}-fout`).textContent = melding;

    if (melding) {
        veld.setAttribute('aria-invalid', 'true');
    } else {
        veld.removeAttribute('aria-invalid');
    }
};

const valideerVeld = (veld) => {
    const melding = controles[veld.id](veld.value);
    toonFout(veld, melding);
    return melding === '';
};

const verwerkFormulier = (event) => {
    event.preventDefault();

    const formulier = event.target;
    const status = document.querySelector('#formulier-status');
    const velden = [...formulier.querySelectorAll('input, textarea')];
    const ongeldig = velden.filter((veld) => !valideerVeld(veld));

    if (ongeldig.length > 0) {
        status.textContent = '';
        ongeldig[0].focus();
        return;
    }

    const naam = document.querySelector('#naam').value.trim();
    status.textContent = `Bedankt ${naam}! Je bericht is goed ingevuld.`;
    formulier.reset();
};

const controleerOpnieuw = (event) => {
    if (event.target.getAttribute('aria-invalid') === 'true') {
        valideerVeld(event.target);
    }
};

const startContact = () => {
    const formulier = document.querySelector('#contactformulier');

    formulier.noValidate = true;
    formulier.addEventListener('submit', verwerkFormulier);
    formulier.addEventListener('input', controleerOpnieuw);
};

startContact();
