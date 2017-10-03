# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure(2) do |config|

  config.vm.box = "ubuntu/xenial64"
  config.vm.network "forwarded_port", guest: 8080, host: 8080

  config.vm.provider "virtualbox" do |v|
    v.memory = 2048
  end

  config.vm.provision "docker", run: "always" do |docker|
    docker.build_image "/vagrant/", args: "--tag vagrant-atlas"
  end

  config.vm.provision "docker", run: "always" do |docker|
    docker.run "vagrant-atlas", args: "-p 8080:5000"
  end

  config.vm.provision "run",
    type: "shell",
    privileged: false,
    run: "always",
    inline: <<-SHELL

    echo "========================================"
    echo "Open a browser at http://localhost:8080/"
    echo "========================================"

  SHELL

end