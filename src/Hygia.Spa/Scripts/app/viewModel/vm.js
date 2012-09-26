define('vm', ['vm.faults','vm.fault','vm.shell'],
    function (faults, fault, shell) {    
        return {
            faults: faults,
            fault: fault,
            shell: shell
        };
    });