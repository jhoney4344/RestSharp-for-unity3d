require 'mkmf'
require 'tmpdir'

# This is needed by the gem, we don't
# care about it though.
create_makefile('Makefile')

nuget = find_executable('nuget')
abort 'Missing NuGet command line tool' if nuget.nil?

xbuild = find_executable('xbuild')
msbuild = find_executable('msbuild')

builder = xbuild || msbuild
abort 'No command line build tools [xbuild,msbuild] available' if builder.nil?

solution_path = File.join('..','..')
project_path = File.join(solution_path,'RestSharp')
build_path = File.join(project_path,'bin','Release')

#
# Fetch remote dependencies
#
solution_makefile = File.join(solution_path,'RestSharp.Mono.sln')
dependency_output = `#{nuget} restore #{solution_makefile}`
MakeMakefile::Logging.message(dependency_output)

#
# Compile the DLL
#
project_makefile = File.join(project_path,'RestSharp.csproj')
build_output = `#{builder} /p:Configuration=Release #{project_makefile}`
MakeMakefile::Logging.message(build_output)

#
# Move the DLL into Unity space as needed
#
if ENV['UNITY_DIR'].nil? == false
	Dir.foreach(build_path) do |it|
		if it.end_with?('.dll')
			output = File.join(build_path,it)
			FileUtils.cp("#{output}",ENV['UNITY_DIR'])
		end
	end
end
