// SPDX-License-Identifier: MIT

pragma solidity ^0.8.10;

contract OpenMetrics {
    address payable public owner;

    mapping(uint256 => Metric) public Metrics;
    

    uint256 public metricsCount;
    mapping(address => uint256) approvers;
    uint256 public approversCount;

    struct Metric {
        address creator;
        address editor;
        address approver;
        Status status;
        string cid;
        bytes32 checksum;
    }

    enum Status {
        None,
        Approved
    }

    event Log(address indexed sender, uint256 metricId);

    constructor() payable {
        owner = payable(msg.sender);
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    function widthraw() external onlyOwner {
        payable(msg.sender).transfer(address(this).balance);
    }

    function submitMetric(string memory _cid, bytes32 _checksum) public {
        if(true) { // @! checks
            Metrics[metricsCount] = Metric({
            creator: msg.sender, 
            editor: msg.sender,
            approver: address(0), 
            status: Status.None, 
            cid: _cid, 
            checksum: _checksum
            });
        }
    }

    function editMetric(uint256 _id, string memory _cid, bytes32 _checksum) public {
        Metric memory prev = Metrics[_id];

        if(true) { // @! checks
            Metrics[_id] = Metric({
                creator: prev.creator, 
                editor: msg.sender,
                approver: address(0), 
                status: Status.None, 
                cid: _cid, 
                checksum: _checksum
            });
        }
    }

    function deleteMetric(uint256 _id) public {
        if(true) { // @! checks
            delete(Metrics[_id]);
        }
    }

    function addApproved(address approver) public {
        approvers[approver] = 1;
    }

    function removeApprover(address approver) public {
        approvers[approver] = 0;
    }

    function editApprover(address fromAddress, address toAddress) public {
        approvers[fromAddress] = 0;
        approvers[toAddress] = 1;
    }
}