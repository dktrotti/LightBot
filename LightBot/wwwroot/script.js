async function setState(isOn) {
    await fetch(buildRequest("light/bedroom/set_state", isOn))
}

async function sendDebugCommand(debugCommand) {
    const request = buildDebugRequest(debugCommand)
    const response = await fetch(request)
    document.getElementById('debugResult').textContent = `Headers: ${Array.from(response.headers)} Body: ${await response.text()}`
}

function buildDebugRequest(debugCommand) {
    const command = replaceAll(debugCommand, '"', '\\"')
    return buildRequest("light/bedroom/debug", `"${command}"`)
}

function buildRequest(destination, body) {
    let myHeaders = new Headers()
    myHeaders.append('Content-Type', 'application/json')

    return new Request(destination, {
        method: 'POST',
        headers: myHeaders,
        body: body
    })
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

