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
    }

    enum Status {
        None,
        Posted,
        Approved,
        Rejected
    }

    event Transaction (
        uint256 id,
        // address author,
        // TransactionType changeType,
        string cid
    );

    enum TransactionType {
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

            Metrics[metricsCount] = Metric({
                creator: msg.sender, 
                editor: msg.sender,
                approver: address(0), 
                status: Status.Posted, 
                cid: _cid, 
                checksum: _checksum
            });

            emit Transaction(metricsCount, 
                            // msg.sender, 
                            // TransactionType.Add, 
                            _cid);
        }
    }

    // function editMetric(uint256 _id, string memory _cid, bytes32 _checksum) public {
    //     Metric storage prev = Metrics[_id];

    //     if(true) { // @! do checks

    //         // Metrics[_id] = Metric({
    //         //     creator: prev.creator, 
    //         //     editor: msg.sender,
    //         //     approver: address(0), 
    //         //     status: Status.None, 
    //         //     cid: _cid, 
    //         //     checksum: _checksum
    //         //     // history: new Change[](0)
    //         // });
    //     }
    // }

    function deleteMetric(uint256 _id) public {
        if(true) { // @! checks
            delete(Metrics[_id]);
        }
    }

    function approveMetric(uint256 _id) public {
        if(true) {
            Metrics[_id].status = Status.Approved;
        }
    }

    function rejectMetric(uint256 _id) public {
        if(true) {
            Metrics[_id].status = Status.Rejected;
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

    function countApproved() public view returns (uint256) {
        uint256 count = 0;
        for(uint256 i = 0; i<metricsCount; i++) {
            if(Metrics[i].status == Status.Approved) {
                count ++;
            }
        }
        return count;
    }

    function metricsApproved() public view returns (uint256[] memory) {
        uint256 count = countApproved();
        uint256[] memory approves = new uint256[](count);
        uint256 j = 0;
        for(uint256 i = 0; i<metricsCount; i++) {
            if(Metrics[i].status == Status.Approved) {
                approves[j] = i;
            }
            j++;
        }
        return approves;
    }

    function countPosted() public view returns (uint256) {
        uint256 count = 0;
        for(uint256 i = 0; i<metricsCount; i++) {
            if(Metrics[i].status == Status.Posted) {
                count ++;
            }
        }
        return count;
    }

    function metricsPosted() public view returns (uint256[] memory) {
        uint256 count = countPosted();
        uint256[] memory posts = new uint256[](count);
        uint256 j = 0;
        for(uint256 i = 0; i<metricsCount; i++) {
            if(Metrics[i].status == Status.Posted) {
                posts[j] = i;
            }
            j++;
        }
        return posts;
    }
}