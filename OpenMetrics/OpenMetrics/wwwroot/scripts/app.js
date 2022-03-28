window.switchChain = async function (chainId) {
    console.log('switch chain called');
    await window.ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [{ chainId: chainId }],
    });
}