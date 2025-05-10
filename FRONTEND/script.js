async function processText() {
    const blacklist = document.getElementById('blacklist').value;
    const inputText = document.getElementById('inputText').value;

    const response = await fetch('http://localhost:5257/censor/gonb', {
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

function createBadge(text, color) {
    const span = document.createElement('span');
    span.className = `badge bg-${color} mx-1`;
    span.textContent = text;
    return span;
}

function displayFrequencies(elementId, freqList) {
    const ul = document.getElementById(elementId);
    ul.innerHTML = '';

    const max = freqList.length > 0 ? freqList[0].count : 1;

    freqList.forEach(freq => {
        const li = document.createElement('li');
        li.textContent = `${freq.word} (${freq.count})`;
        li.style.fontSize = `${1 + (freq.count / max) * 1.5}em`;
        ul.appendChild(li);
    });
}
function extractCleanText(textWithBadges) {
    return textWithBadges.replace(/\(([^|]+)\|([^)]+)\)/g, '$2');
}