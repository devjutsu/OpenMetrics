// SPDX-License-Identifier: MIT

pragma solidity ^0.8.10;

contract OpenMetrics {
    address payable public owner;

    mapping(uint256 => Metric) public Metrics;
    uint256 public articlesCount;
    mapping(address => uint256) approvers;
    uint256 public approversCount;

    struct Metric {
        address creator;
        address approver;
        Status status;
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

    function submitArticle() public {
        
    }

    function editArticle() public {

    }

    function addApproved(address approver) public {
        approvers[approver] = 1;
    }

    function removeApprover(address approver) public {
        approvers[approver] = 0;
    }

    function editApprover(address fromAddress, address toAddress) public {
        approvers[fromAddress] = 0;
        approvers[toAddress] = 0;
    }
}