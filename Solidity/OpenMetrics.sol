// SPDX-License-Identifier: MIT

pragma solidity >=0.7.0 <0.9.0;

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
        // mapping(uint256 => Change) transactions;
        // uint256 transactionCount;
    }

    enum Status {
        None,
        Posted,
        Approved
    }

    struct Change {
        address author;
        // datetime
        ChangeType changeType;
    }

    enum ChangeType {
        Add,
        Update,
        Approve
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
            metricsCount++;

            //Change memory change = Change({author: msg.sender, changeType: ChangeType.Add});
            // Change[] memory hist = new Change[](1);
            // hist.push(Change({author: msg.sender, changeType: ChangeType.Add}));

            Metrics[metricsCount] = Metric({
                creator: msg.sender, 
                editor: msg.sender,
                approver: address(0), 
                status: Status.Posted, 
                cid: _cid, 
                checksum: _checksum
                //history: null,
                // transactionCount: 0
            });
        }
    }

    function editMetric(uint256 _id, string memory _cid, bytes32 _checksum) public {
        Metric storage prev = Metrics[_id];

        if(true) { // @! do checks

            Metrics[_id] = Metric({
                creator: prev.creator, 
                editor: msg.sender,
                approver: address(0), 
                status: Status.None, 
                cid: _cid, 
                checksum: _checksum
                // history: new Change[](0)
            });
        }
    }

    function deleteMetric(uint256 _id) public {
        if(true) { // @! checks
            delete(Metrics[_id]);
        }
    }

    function approveMetric(uint256 _id) public {
        if(true) {
            Metric storage metric = Metrics[_id];
            metric.status = Status.Approved;
            Metrics[_id] = metric;
        }
    }

    function getMetric(uint256 _id) public view returns (string memory) {
        return Metrics[_id].cid;
    }

    function addApprover(address approver) public {
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