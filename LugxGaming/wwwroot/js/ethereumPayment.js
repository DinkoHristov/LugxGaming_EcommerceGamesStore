document.addEventListener('DOMContentLoaded', () => {
    const payWithEthereumButton = document.getElementById('payWithEthereum');

    if (payWithEthereumButton) {
        payWithEthereumButton.addEventListener('click', async () => {
            const amountInEthers = payWithEthereumButton.getAttribute('data-amount');

            // Fetch the wei amount from the new endpoint
            const response = await fetch(`/Payment/GetAmountInWei?amount=${amountInEthers}`);
            if (!response.ok) {
                throw new Error('Failed to get the amount in wei');
            }
            const amountInWei = await response.text();

            if (typeof window.ethereum !== 'undefined') {
                try {
                    const accounts = await ethereum.request({ method: 'eth_accounts' });
                    if (accounts.length === 0) {
                        await ethereum.request({ method: 'eth_requestAccounts' });
                    }

                    const account = accounts[0];
                    const recipientAddress = '0x3d43efba99617e31c1d3770137a820973d7b524c'; // My MetaMask address

                    // Send transaction
                    const transactionParameters = {
                        to: recipientAddress,
                        from: account,
                        value: amountInWei,
                        gas: '0x5028',
                    };

                    var result = await ethereum.request({
                        method: 'eth_sendTransaction',
                        params: [transactionParameters],
                    });

                    console.log('Transaction result:', result);

                    window.location.href = '/Payment/PaymentSuccess';
                } catch (error) {
                    console.error('Error with MetaMask transaction:', error);
                    alert(`Error with MetaMask transaction: ${error.message}`);
                }
            } else {
                alert('MetaMask is not installed or not detected. Please install MetaMask and try again.');
            }
        });
    }
});
