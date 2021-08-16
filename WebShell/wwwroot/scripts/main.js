let input = document.getElementById('input');
let output = document.getElementById('output');
let dir = document.getElementById('currentDir');
let currentComm = -1;
let path = "";

async function onSubmit(e) {
    if (e.keyCode === 13) {
        console.log(path);
        await fetch(`home/output?command=${input.value}`)
            .then(data => data.json())
            .then(data => {
                output.innerHTML += `${dir.innerHTML}${input.value}<br>${data[0]}<br>`;
                path = data[1];
                dir.innerHTML = path + ">";
                input.scrollIntoView();
            });
        input.value = null;
        currentComm = -1;
    }
}

document.addEventListener(
    'keydown',
    async (e) => {
        input.focus();
        if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
            await fetch('home/history')
                .then(data => data.json())
                .then(data => {
                    var len = data.length - 1;
                    if (e.key === 'ArrowUp') {
                        if (currentComm !== len)
                            currentComm++;
                    }
                    else {
                        if (currentComm !== 0)
                            currentComm--;
                    }
                    input.value = data[len - currentComm].text;
                });
        }
    }
);