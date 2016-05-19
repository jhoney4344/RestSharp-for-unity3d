Gem::Specification.new do |s|
	s.name = 'rest-client-unity'
	s.version = '0.0.0'
	s.date = ENV['DELIVER_DATE'] || Date.today()
	s.summary = 'REST client for use in Unity projects'
	s.description = "A .NET 2.0 library to encapsulate REST service interaction"
	s.authors = ['LlamaZOO']
	s.email = 'info@llamazoo.com'
	s.license = 'Nonstandard'
	s.homepage = 'http://www.llamazoo.com'

	s.files = Dir['./RestSharp/**/*']
	s.files += ['RestSharp.Mono.sln']

	s.files.reject! { |file_name|
		file_name.start_with?('./src/bin') || file_name.start_with?('./src/obj')
	}

	s.extensions = %w[ext/rest/extconf.rb]
end