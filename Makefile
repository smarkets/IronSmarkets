.PHONY: all
all: compile

.PHONY: compile
compile: net40 net35

.PHONY: net40
net40:
	xbuild /verbosity:minimal /tv:4.0 /p:TargetFrameworkProfile="" /p:Framework="NET40" IronSmarkets/IronSmarkets.csproj
	xbuild /verbosity:minimal /tv:4.0 /p:TargetFrameworkProfile="" /p:Framework="NET40" IronSmarkets.ConsoleExample/IronSmarkets.ConsoleExample.csproj
	xbuild /verbosity:minimal /tv:4.0 /p:TargetFrameworkProfile="" /p:Framework="NET40" IronSmarkets.Tests/IronSmarkets.Tests.csproj

.PHONY: net35
net35:
	xbuild /verbosity:minimal /tv:3.5 /p:TargetFrameworkProfile="" /p:TargetFrameworkVersion="v3.5" /p:Framework="NET35" IronSmarkets/IronSmarkets.csproj
	xbuild /verbosity:minimal /tv:3.5 /p:TargetFrameworkProfile="" /p:TargetFrameworkVersion="v3.5" /p:Framework="NET35" IronSmarkets.ConsoleExample/IronSmarkets.ConsoleExample.csproj
	xbuild /verbosity:minimal /tv:3.5 /p:TargetFrameworkProfile="" /p:TargetFrameworkVersion="v3.5" /p:Framework="NET35" IronSmarkets.Tests/IronSmarkets.Tests.csproj

.PHONY: clean
clean:
	rm -rf IronSmarkets/bin IronSmarkets/obj
	rm -rf IronSmarkets.Tests/bin IronSmarkets.Tests/obj
	rm -rf IronSmarkets.ConsoleExample/bin IronSmarkets.ConsoleExample/obj

.PHONY: distclean
distclean: clean
	git clean -f -x -d

.PHONY: test
test: all
	mono --verify-all bin/xunit.console.clr4.exe IronSmarkets.Tests/bin/Debug/IronSmarkets.Tests.dll /nunit NUnitTestResult.xml /silent
