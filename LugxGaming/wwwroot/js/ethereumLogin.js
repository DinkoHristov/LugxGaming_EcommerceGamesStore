document.addEventListener('DOMContentLoaded', async () => {
    const connectButton = document.getElementById('connectMetaMask');
    const logoutButton = document.getElementById('logoutMetaMask');

    connectButton.addEventListener('click', async () => {
        await connectMetaMask();
    });

    logoutButton.addEventListener('click', async () => {
        await logoutMetaMask();
    });

    if (typeof window.ethereum !== 'undefined') {
        console.log('MetaMask is installed!');
        checkAccount();
    } else {
        console.log('MetaMask is not installed. Please install it to use this feature.');
    }

    async function checkAccount() {
        const response = await fetch('/Payment/GetAccount');
        if (response.ok) {
            const account = await response.text();
            if (account) {
                console.log('Account already connected:', account);
                connectButton.textContent = `Connected: ${account.substring(0, 6)}...${account.substring(account.length - 4)}`;
            }
        }
    }

    async function connectMetaMask() {
        if (typeof window.ethereum !== 'undefined') {
            try {
                await window.ethereum.request({
                    method: 'wallet_requestPermissions',
                    params: [{
                        eth_accounts: {}
                    }]
                });

                const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
                const account = accounts[0];

                const web3 = new Web3(window.ethereum);
                const balance = await web3.eth.getBalance(account);

                alert(`Connected account: ${account}\nBalance: ${web3.utils.fromWei(balance, 'ether')} ETH`);

                saveAccountInSession(account);
            } catch (error) {
                console.error('User denied account access or error occurred:', error);
            }
        } else {
            alert('MetaMask is not installed. Please install MetaMask and try again.');
            window.open('https://metamask.io/download.html', '_blank');
        }
    }

    function saveAccountInSession(account) {
        fetch('/Payment/SaveAccount', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ account })
        }).then(response => {
            if (response.ok) {
                console.log('Account saved in session');
                checkAccount();
            } else {
                console.error('Error saving account in session');
            }
        });
    }

    async function logoutMetaMask() {
        // Call your backend to remove the session
        const response = await fetch('/Payment/LogoutAccount', {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        });

        if (response.ok) {
            if (typeof window.ethereum !== 'undefined') {
                try {
                    const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
                    const account = accounts[0];

                    console.log(`Account: ${account} has logged out!`);
                    alert(`Account: ${account} has logged out!`);
                    connectButton.textContent = 'Connect MetaMask';
                } catch (error) {
                    console.error('Error logging out from MetaMask:', error);
                }
            }
        } else {
            console.error('Cannot find user to logged out!');
        }
    }
});
