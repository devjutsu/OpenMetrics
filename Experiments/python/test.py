from web3 import Web3
from web3._utils.events import get_event_data

w3 = Web3(Web3.HTTPProvider("<Infura host>"))
contract = w3.eth.contract(address="0x33..", abi=abi['abi'])
event_template = contract.events.<EVENT_NAME>
events = w3.eth.get_logs({'fromBlock':from_block, 'toBlock': from_block+10000, 'address':"0x33.."})


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