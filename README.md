# Simple Vagrant Box Server

This project is in no way related to HashiCorp nor Mitchell Hashimoto (mitchellh).

Proof of concept to host on-premise vagrant boxes. The server reuse the Box Catalog Format from Hashicorp Atlas and allows
boxes to be discoverable, versioned and updatable.

See official sources for more information: https://www.vagrantup.com/docs/boxes/format.html

I did not invent anything, others chewed everything for me and credits go to them
- https://github.com/mitchellh/vagrant
- https://github.com/hollodotme/Helpers/blob/master/Tutorials/vagrant/self-hosted-vagrant-boxes-with-versioning.md

## Customize and Deploy

To be production-ready, this project requires a more robust implementation of IBoxRepository.
I only provide an in-memory implementation or a json backed file storage. You should also
customize Startup.cs to fit your needs. You could replace the very simple
SingletonRepositoriesHttpControllerActivator.cs by a production DI/IoC container
or simply embed the library (VagrantAtlas) in your own ASP.NET project.

This project only protects __PUT__ calls with Basic Authentication. Currently, only a single
user is supported and configured in web.config.

## Endpoints
### __GET /__ (defaults to /atlas)
### __GET /atlas__

Returns all boxes from the box repository.  It is not used by vagrant.

### __GET /boxes/user/box_name__

Returns the metadata.json used by vagrant to download a vagrant box.

### __PUT /boxes/user/box_name/version__

Add or Updates the provider sent in the request body and returns the updated metadata.json.
Example payload:

    {
      "name": "virtualbox",
      "url": "http://your.server.domain/path/to/boxes/my.box",
      "checksum_type": "sha256",
      "checksum": "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
    }

The box is not sent, only a pointer to itself. It is your duty
to make sure the box is available at this url and to compute the checksum.
The url will by poked by the vagrant client on *vagrant init*.

## Integration with vagrant (command-line)

To use your newly published box:

    vagrant init box_name http://your.server.domain/api_location/boxes/user/box_name

To edit an existing VagrantFile, update the files

    # Every Vagrant development environment requires a box. You can search for
    # boxes at https://atlas.hashicorp.com/search.
    config.vm.box = "box_name"

    # The url from where the 'config.vm.box' box will be fetched if it
    # doesn't already exist on the user's system.
    config.vm.box_url = "http://your.server.domain/api_location/boxes/user/box_name"
