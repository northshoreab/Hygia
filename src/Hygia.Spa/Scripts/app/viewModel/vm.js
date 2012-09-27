define('vm', ['vm.faults','vm.fault','vm.shell', 'vm.signup', 'vm.faultoverview'],
    function (faults, fault, shell, signUp, faultOverview) {    
        return {
            faults: faults,
            fault: fault,
            shell: shell,
            signUp: signUp,
            faultOverview: faultOverview
        };
    });