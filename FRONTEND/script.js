async function processText() {
    const blacklist = document.getElementById('blacklist').value;
    const inputText = document.getElementById('inputText').value;

    const response = await fetch('https://localhost:5001/censor', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            blacklistText: blacklist,
            inputText: inputText
        })
    });

    const data = await response.json();
    displayModifiedText(data.modifiedText);
    displayFrequencies('originalFreq', data.originalFrequencies);
    displayFrequencies('modifiedFreq', data.modifiedFrequencies);
}
function displayModifiedText(text) {
    const resultDiv = document.getElementById('result');
    resultDiv.innerHTML = '';

    const pattern = /\(([^|]+)\|([^)]+)\)/g;
    let lastIndex = 0;
    let match;

    while ((match = pattern.exec(text)) !== null) {
        // Szöveg a találat előtt
        resultDiv.append(document.createTextNode(text.slice(lastIndex, match.index)));

        const original = match[1];
        const replacement = match[2];

        const originalBadge = createBadge(original, 'danger');
        const replacementBadge = createBadge(replacement, 'success');

        resultDiv.append(originalBadge, ' → ', replacementBadge);

        lastIndex = pattern.lastIndex;
    }

    // Maradék szöveg
    resultDiv.append(document.createTextNode(text.slice(lastIndex)));
}

