pipeline
{
    agent any
    stages
    {       
        stage('Build')
        {
            steps
            {
                script {
                def msbuild = tool name: 'MSBuild', type: 'hudson.plugins.msbuild.MsBuildInstallation'
                bat "${msbuild} Chatter.sln /t:Restore /p:Configuration=Release"
                bat "${msbuild} Chatter.sln /p:Configuration=Release"
                } 
                
            }
        }
        
        stage('Test')
        {
            steps
            {
                bat "dotnet test .\\Chatter.Core.Tests\\Chatter.Core.Tests.csproj"
            }
        }
    }
}