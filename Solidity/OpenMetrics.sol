// SPDX-License-Identifier: MIT

pragma solidity >=0.7.0 <0.9.0;

contract OpenMetrics {
    address payable public owner;

    mapping(uint256 => Metric) public Metrics;
    uint256 public metricsCount;
    
    mapping(address => uint256) approvers;
    uint256 public approversCount;

    mapping(uint256 => MetricHistory) History;

    struct Metric {
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

    struct MetricHistory {
        mapping(uint256 => HistoryRecord) records;
        uint256 count;
    }

    struct HistoryRecord {

        uint256 id;
        address author;
        Status status;
        uint256 timestamp;
        // string cid;
    }

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
                status: Status.Posted, 
                cid: _cid, 
                checksum: _checksum
            });

            History[metricsCount].records[0].status = Status.Posted;
            History[metricsCount].records[0].author = msg.sender;
            History[metricsCount].records[0].timestamp = block.timestamp;
            History[metricsCount].count = 1;

            metricsCount++;
        }
    }

    function getHistoryRecordCount(uint256 _id) public view returns (uint256) {
        return History[_id].count;
    }

    function getHistoryRecord(uint256 _id, uint256 _n) public view returns (HistoryRecord memory) {
        return History[_id].records[_n];
    }

    function historyRecord(uint256 _id, uint256 _n) public view returns (HistoryRecord memory) {
        return History[_id].records[_n];
    }

    function getHistory(uint256 _id) public view returns (HistoryRecord[] memory history) {
        uint256 count = History[_id].count;

        HistoryRecord[] memory records = new HistoryRecord[](count);
        for(uint256 i = 0; i < count; i++) {
            records[i].status = History[_id].records[i].status;
            records[i].author = History[_id].records[i].author;
            records[i].timestamp = History[_id].records[i].timestamp;
        }
        return records;
    }


    function deleteMetric(uint256 _id) public {
        if(true) { // @! checks
            delete(Metrics[_id]);
        }
    }

    function approveMetric(uint256 _id) public {
        if(true) {
            Metrics[_id].status = Status.Approved;

            uint256 count = History[_id].count;
            History[_id].records[count].id = count;
            History[_id].records[count].status = Status.Approved;
            History[_id].records[count].author = msg.sender;
            History[_id].records[count].timestamp = block.timestamp;

            History[_id].count = count + 1;
        }
    }

    function rejectMetric(uint256 _id) public {
        if(true) {
            Metrics[_id].status = Status.Rejected;

            uint256 count = History[_id].count;
            History[_id].records[count].id = count;
            History[_id].records[count].status = Status.Rejected;
            History[_id].records[count].author = msg.sender;
            History[_id].records[count].timestamp = block.timestamp;

            History[_id].count = count + 1;
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
        for(uint256 i = 0; i < metricsCount; i++) {
            if(Metrics[i].status == Status.Approved) {
                count ++;
            }
        }
        return count;
    }

    function metricsApproved() public view returns (uint256[] memory approved) {
        uint256 count = countApproved();
        uint256[] memory approves = new uint256[](count);
        uint256 j = 0;
        for(uint256 i = 0; i < metricsCount; i++) {
            if(Metrics[i].status == Status.Approved && j < count) {
                approves[j] = i;
                j++;
            }
        }

        // uint256[] memory approves = new uint256[](3);
        // approves[0] = 5;
        // approves[1] = 7;
        // approves[2] = 9;
        return approves;
    }

    function countUnchecked() public view returns (uint256) {
        uint256 count = 0;
        for(uint256 i = 0; i < metricsCount; i++) {
            if(Metrics[i].status == Status.Posted) {
                count ++;
            }
        }
        return count;
    }

    function metricsUnchecked() public view returns (uint256[] memory unverified) {
        uint256 count = countUnchecked();
        uint256[] memory data = new uint256[](count);
        uint256 j = 0;
        for(uint256 i = 0; i < metricsCount; i++) {
            if(Metrics[i].status == Status.Posted) {
                data[j] = i;
            }
            j++;
        }

        // uint256[] memory data = new uint256[](metricsCount);  // -- working sample data
        // for(uint i = 0; i < data.length; i++) {
        //     data[i] = i;
        // }

        return data;
    }

    function clean() public {
        metricsCount = 0;
        approversCount = 0;
    }
}