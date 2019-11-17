async function sendDebugCommand(debugCommand) {
    const request = buildDebugRequest(debugCommand)
    const response = await fetch(request)
    document.getElementById('debugResult').textContent = `Headers: ${Array.from(response.headers)} Body: ${await response.text()}`
}

function buildDebugRequest(debugCommand) {
    let myHeaders = new Headers()
    myHeaders.append('Content-Type', 'application/json')

    const command = replaceAll(debugCommand, '"', '\\"')
    console.log(command)

    return new Request("light", {
        method: 'POST',
        headers: myHeaders,
        body: `"${command}"`
    })
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

