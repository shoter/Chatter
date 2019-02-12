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
                echo "Branch name: ${branch_name}"
                bat "\"${tool 'MsBuild'}MsBuild.exe\" Chatter.sln /t:Restore /p:Configuration=Release"
                bat "\"${tool 'MsBuild'}MSBuild.exe\" Chatter.sln /p:Configuration=Release"
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

        stage('Merge to master')
        {
            when
            {
                expression { "${branch_name}" == "develop" }
            }
            steps
            {
                echo "Tutaj bym mergowal"
            }
        }
    }
}