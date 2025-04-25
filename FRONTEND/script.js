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