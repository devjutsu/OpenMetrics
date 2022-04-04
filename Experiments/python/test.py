from web3 import Web3
from web3._utils.events import get_event_data



f = open("abi.txt", "r")
abi = f.read()

w3 = Web3(Web3.HTTPProvider("https://polygon-mumbai.infura.io/v3/b13dfe01d1234fb9a273c558d636d4ea"))
contract = w3.eth.contract(address="0xA3ad2738d17Eccd72AD9b3B2e51e099F96FAB9C8", abi=abi) #['abi']
event_template = contract.events.Log
events = w3.eth.get_logs({'fromBlock':25801255, 'toBlock': 25801300, 'address':"0xA3ad2738d17Eccd72AD9b3B2e51e099F96FAB9C8"})


def handle_event(event, event_template):
    try:
        result = get_event_data(event_template.web3.codec, event_template._get_event_abi(), event)
        return True, result
    except:
        return False, None

for event in events: 
    suc, res = self.handle_event(event=event, event_template=event_template)   
    if suc:
        print("Event found", res)