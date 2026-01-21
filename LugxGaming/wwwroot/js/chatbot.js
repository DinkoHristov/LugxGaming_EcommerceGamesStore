//const chatBotWindow = document.getElementById("chatbotWindow");
//const input = document.getElementById("chatbotInput");
//const messages = document.getElementById("chatbotMessages");

//document.querySelector('.chatbot-toggle')?.addEventListener('click', (e) => {
//    e.preventDefault();

//    chatBotWindow.classList.toggle('hidden');
//});

//document.getElementById('chatBotBtn')?.addEventListener('click', (e) => {
//    e.preventDefault();

//    chatBotWindow.classList.toggle('hidden');
//});

//document.getElementById('chatBotMsg')?.addEventListener('click', askAIChatBotQuestion);

//async function askAIChatBotQuestion() {
//    const requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
//    const message = input.value.trim();
//    if (!message)
//        return;

//    messages.innerHTML += `<div class="user">${message}</div>`;

//    try {
//        const response = await fetch('/ChatBot/GetChatBotResponse', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json',
//                'RequestVerificationToken': requestVerificationToken
//            },
//            body: JSON.stringify(message)
//        });

//        const data = await response.json();
//        if (!data.ok) {
//            throw new Error(`${data.response}`);
//        }

//        messages.innerHTML += `<div class="bot">${data.response}</div>`;
//        messages.scrollTop = messages.scrollHeight;
//        input.value = '';
//    } catch (error) {
//        input.value = '';
//        alert(error.message);
//    }
//}

const chatBotWindow = document.getElementById("chatbotWindow");
const input = document.getElementById("chatbotInput");
const messages = document.getElementById("chatbotMessages");

document.querySelector('.chatbot-toggle')?.addEventListener('click', (e) => {
    e.preventDefault();

    chatBotWindow.classList.toggle('hidden');
});

document.getElementById('chatBotBtn')?.addEventListener('click', (e) => {
    e.preventDefault();

    chatBotWindow.classList.toggle('hidden');
});

document.getElementById('chatBotMsg')?.addEventListener('click', askAIChatBotQuestion);

async function askAIChatBotQuestion() {
    const requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
    const message = input.value.trim();
    if (!message)
        return;

    messages.innerHTML += `<div class="user">${message}</div>`;

    try {
        const response = await fetch('/ChatBot/GetChatBotResponse', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': requestVerificationToken
            },
            body: JSON.stringify(message)
        });

        const data = await response.json();
        if (!data.ok) {
            throw new Error(`${data.response}`);
        }

        messages.innerHTML += `<div class="bot">${data.response}</div>`;
        messages.scrollTop = messages.scrollHeight;
        input.value = '';
    } catch (error) {
        input.value = '';
        alert(error.message);
    }
}