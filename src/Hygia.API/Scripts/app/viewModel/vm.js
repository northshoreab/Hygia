define('vm', ['vm.faults','vm.fault','vm.shell', 'vm.home', 'vm.faultoverview', 'vm.login'],
    function (faults, fault, shell, home, faultOverview, login) {    
        return {
            faults: faults,
            fault: fault,
            shell: shell,
            home: home,
            faultOverview: faultOverview,
            login: login
        };
    });