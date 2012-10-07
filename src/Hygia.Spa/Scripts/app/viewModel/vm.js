define('vm', ['vm.faults','vm.fault','vm.shell', 'vm.signup', 'vm.faultoverview', 'vm.login'],
    function (faults, fault, shell, signUp, faultOverview, login) {    
        return {
            faults: faults,
            fault: fault,
            shell: shell,
            signUp: signUp,
            faultOverview: faultOverview,
            login: login
        };
    });